import { Injectable } from '@angular/core';
import * as signalR from '@microsoft/signalr';

@Injectable({ providedIn: 'root' })
export class TaskSignalRService {
  private hubConnection!: signalR.HubConnection;

  public startConnection(username: string): void {
    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl('http://localhost:5206/taskHub', {
        accessTokenFactory: () => localStorage.getItem('token') || ''
      })
      .build();

    this.hubConnection
      .start()
      .then(() => {
        console.log('SignalR connection started');
        this.hubConnection.invoke("AddToGroup", username); // Optional if you manage groups
      })
      .catch(err => console.error('SignalR error:', err));
  }

  public onTaskAssigned(callback: (taskName: string) => void): void {
    this.hubConnection.on('ReceiveTaskNotification', (taskName: string) => {
      callback(taskName);
    });
  }
}
