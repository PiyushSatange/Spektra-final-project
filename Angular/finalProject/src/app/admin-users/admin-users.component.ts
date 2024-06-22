import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-admin-users',
  templateUrl: './admin-users.component.html',
  styleUrl: './admin-users.component.css'
})
export class AdminUsersComponent implements OnInit{
  users : any;

  constructor(private http: HttpClient){}
  ngOnInit(): void {
    this.http.get("https://localhost:7072/api/User/getAll").subscribe((data) => {
      this.users = data;
    })
  }

  blockUser(email: any){
    this.http.delete(`https://localhost:7072/api/User/BlockUser/${email}`).subscribe((data) => {
      console.log(data);
    })
  }
}
