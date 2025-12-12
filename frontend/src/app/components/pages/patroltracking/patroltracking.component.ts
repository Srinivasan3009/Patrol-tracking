import { CommonModule } from '@angular/common';
import { HttpClient, HttpClientModule } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { RoleService } from '../../services/role.service';
import { isPlatformBrowser } from '@angular/common';
import { Inject, PLATFORM_ID } from '@angular/core';
import { LoginService } from '../../services/login/login.service';
import { TaskSignalRService } from '../../services/task-signalr.service';
interface WF {
  wfid:string;
  wftitle: string;           
  description: string;
  status:string;
  start: Date;
  end:Date;
  createdby:string;
  modifiedby: string;
  modifieddate:string;
  active:string;
}
interface Task {
  wfid: string;
  taskid: string;
  taskname: string;
  remarks: string;
  start: string;
  end: string;
  assignedto:string
}
@Component({
  selector: 'app-patroltracking',
  standalone: true,
  imports: [ReactiveFormsModule, CommonModule, HttpClientModule, FormsModule, RouterModule],
  templateUrl: './patroltracking.component.html',
  styleUrl: './patroltracking.component.css'
})
export class PatroltrackingComponent implements OnInit {
showForm = false;
allTasks: Task[] = []; 
  editIndex: number = -1;
  wf: WF[] = [];
  roles: any[]=[];
  AddForm: FormGroup;
  updateForm: FormGroup;
  username: String='';
  taskForm:FormGroup;
  taskid: string='';
  taskupdateForm: FormGroup;
  taskeditIndex: number=-1;
selectedUsername: any;
users: any=this.loginService.getuser();
  constructor(private fb: FormBuilder, private http: HttpClient,@Inject(PLATFORM_ID) private platformId: Object,private loginService: LoginService,private taskSignalRService: TaskSignalRService) {
    this.AddForm = this.fb.group({
      title: ['', Validators.required],
      description:[''],
      start:[null],
      end:[null]
    });
    this.taskForm=this.fb.group({
      taskname:[''],
      remarks:[''],
      start:[null],
      end:[null]
    })
    this.updateForm = this.fb.group({
      wfid:[''],
      wftitle: [''],
      description:[''],
      start:[null],
      end:[null],
      active:[false],
      createdby:['']
    });
    this.taskupdateForm = this.fb.group({
      wfid:[''],
      taskid: [''],
      taskname:[''],
      start:[null],
      end:[null],
      remarks:[''],
      assignedto:['']
    });
  }

  ngOnInit(): void {
    if (isPlatformBrowser(this.platformId)) {
      this.username = this.loginService.getname();
    console.log("username:", this.username);
  }
   this.taskSignalRService.startConnection(this.loginService.getname());

 this.taskSignalRService.onTaskAssigned((taskName: string) => {
  console.log('New Task Assigned:', taskName);
  alert(`You have been assigned: ${taskName}`);
});
  this.http.get<any>('http://localhost:5206/api/User/getuser').subscribe({
  next: (data) => this.users = data,  
  error: (err) => console.error('Error loading users', err)
});
  
    this.load();
  }

  load(): void {
    this.http.get<any>("http://localhost:5206/api/Workflow/getwf").subscribe({
      next: (data) => this.wf = data,
      error: (err) => console.error('Error fetching workflow:', err)
    });
  }

  toggleForm(): void {
    this.showForm = !this.showForm;
  }

  onSubmit(): void {
    const formData = {
      id:"",
      createdby:this.username,
      wfid:"",
      wftitle: this.AddForm.value.title,
      description: this.AddForm.value.description,
      start: this.AddForm.value.start,
      end: this.AddForm.value.end,
      status:"pending",
      modifiedby:"",
      Modifieddate:null,
      active:false,
    };

    this.http.post('http://localhost:5206/api/Workflow/addwf', formData).subscribe({
      next: () => {
        alert('Added successful');
        this.load();
      },
      error: (err) => {
        alert('failed: ' + (err.error?.message || err.message));
      }
    });

    this.AddForm.reset();
    this.showForm = false;
  }

  removeUser(index: number): void {
    const userToDelete = this.wf[index];
    if (!userToDelete) {
      console.error('User not found at index', index);
      return;
    }

    this.http.request('DELETE', `http://localhost:5206/api/Workflow/deletewf/${userToDelete.wfid}`).subscribe({
      next: () => {
        alert('Delete successful');
        this.wf.splice(index, 1);
      },
      error: (err) => {
        alert('Delete failed: ' + (err.error?.message || err.message));
      }
    });
  }

  editUser(index: number): void {
    this.editIndex = index;
    const w = this.wf[index];

    this.updateForm.setValue({
      wfid: w.wfid,
      wftitle: w.wftitle,        
      description: w.description,
      start: w.start,
      end:w.end,
      active:w.active,
      createdby: w.createdby
    });
  }

  CancelEdit(): void {
    this.editIndex = -1;
    this.taskeditIndex=-1;
  }

  Update(): void {
    const updatePayload = {
      wfid: this.updateForm.value.wfid,
       createdby:this.updateForm.value.createdby,
        wftitle: this.updateForm.value.wftitle,
        description: this.updateForm.value.description,
        start: this.updateForm.value.start,
        end: this.updateForm.value.end,
        active: this.updateForm.value.active,
        modifiedby:this.username,
        modifieddate: null,
        status:"pending",
        id:""
    };
    this.http.put("http://localhost:5206/api/Workflow/updatewf", updatePayload).subscribe({
      next: () => {
        alert('Workflow update successful');
        this.load();
        this.editIndex = -1;
      },
      error: (err) => {
        console.error(err);
        alert('Failed to update Workflow: ' + (err.error?.message || err.message));
      }
    });
  }
  showTasks: { [wfid: string]: boolean } = {};
tasksByWorkflow: { [wfid: string]: Task[] } = {};

toggleTasks(wfid: string): void {
  this.showTasks[wfid] = !this.showTasks[wfid];

  if (this.showTasks[wfid]) {
    if (!this.tasksByWorkflow[wfid]) {
      this.fetchTasksForWorkflow(wfid); // will do the filtering inside
    }
  }
}

fetchTasksForWorkflow(wfid: string): void {
  this.http.get<any[]>(`http://localhost:5206/api/Task/gettask`).subscribe({
    next: (data) => {
      this.allTasks = data;
      this.tasksByWorkflow[wfid] = this.allTasks.filter(
        task => task.wfid?.trim() === wfid.trim()
      );
    },
    error: (err) => {
      console.error('Error fetching tasks:', err);
    }
  });
}
wfid: string='';
addIndex: number=-1;
Addtask(i:number){
  const w=this.wf[i];
  this.wfid=w.wfid;
  this.addIndex=i;
  this.showForm=true;
}
submit(){
  const formData = {
      id:"",
      taskid:"",
      taskname:this.taskForm.value.taskname,
      remarks: this.taskForm.value.remarks,
      wfid: this.wfid,
      start: this.taskForm.value.start,
      end: this.taskForm.value.end,
      assignedto:""
    };

    this.http.post('http://localhost:5206/api/Task/addtask', formData).subscribe({
      next: () => {
        alert('Added successful');
        this.addIndex = -1;
        this.fetchTasksForWorkflow(this.wfid);
      },
      error: (err) => {
        alert('failed: ' + (err.error?.message || err.message));
      }
    });
    this.taskForm.reset();
    this.showForm = false;
}
removeTask(index: number): void {
    const taskToDelete = this.allTasks[index];
    this.wfid=taskToDelete.wfid;
    if (!taskToDelete) {
      console.error('User not found at index', index);
      return;
    }
    
    this.taskid=taskToDelete.taskid;
    this.http.request('DELETE', `http://localhost:5206/api/Task/deletetask/${this.taskid}`).subscribe({
      next: () => {
        alert('Delete successful');
        this.allTasks.splice(index, 1);
        this.fetchTasksForWorkflow(this.wfid);
      },
      error: (err) => {
        alert('Delete failed: ' + (err.error?.message || err.message));
      }
    });
  }

  editTask(index: number): void {
    this.taskeditIndex = index;
    const task = this.allTasks[index];
    this.taskid=task.taskid;
    this.wfid=task.wfid;
    this.taskupdateForm.setValue({
      wfid:task.wfid,
      taskid: task.taskid,
      taskname: task.taskname,
      remarks: task.remarks,
      start:task.start,
      end:task.end,
      assignedto:task.assignedto
    });
  }

  UpdateTask(): void {
    const updatePayload = {
      id:"",
      taskid: this.taskid,
      taskname:this.taskupdateForm.value.taskname,
      remarks: this.taskupdateForm.value.remarks,
      wfid: this.wfid,
      start: this.taskupdateForm.value.start,
      end: this.taskupdateForm.value.end,
      assignedto:""
    };
    
    this.http.put("http://localhost:5206/api/Task/updatetask", updatePayload).subscribe({
      next: () => {
        alert('Task update successful');
        this.fetchTasksForWorkflow(this.wfid);
        this.taskeditIndex = -1;
      },
      error: (err) => {
        console.error(err);
        alert('Failed to update Task: ' + (err.error?.message || err.message));
      }
    });
  }
  assignTask(taskid: string) {
  const payload = {
    taskid: taskid,
    assignedto: this.selectedUsername  // e.g., 'srini'
  };

  this.http.put('http://localhost:5206/api/Task/assigntask', payload).subscribe({
    next: () => alert('Task assigned'),
    error: err => alert('Failed: ' + (err.error?.message || err.message))
  });
}
confirmAssign(): void {
  if (!this.selectedTaskId || !this.selectedUsername) {
    alert('Please select both task and user.');
    return;
  }

  const payload = {
    taskname: this.selectedTaskId,
    name: this.selectedUsername
  };

  this.http.put('http://localhost:5206/api/Task/assigntask', payload).subscribe({
    next: () => {
      alert('Task assigned successfully');
      this.cancelAssign();
    },
    error: (err) => {
      alert('Failed to assign task: ' + (err.error?.message || err.message));
    }
  });
}
showAssignPopup = false;
selectedTaskId = ''; // Filled with users from API

openAssignPopup(): void {
  if (!this.allTasks || this.allTasks.length === 0) {
    this.http.get<any[]>(`http://localhost:5206/api/Task/gettask`).subscribe({
    next: (data) => {
      this.allTasks=data; 
    },error:(err)=>{
      console.log(err.error.message);
    }
    });
    }
  this.showAssignPopup = true;
}
cancelAssign(): void {
  this.showAssignPopup = false;
  this.selectedTaskId = '';
  this.selectedUsername = '';
}

}

