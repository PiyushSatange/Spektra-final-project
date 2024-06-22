import { HttpClient } from '@angular/common/http';
import { Component } from '@angular/core';

@Component({
  selector: 'app-user-dashboard',
  templateUrl: './user-dashboard.component.html',
  styleUrl: './user-dashboard.component.css'
})
export class UserDashboardComponent {
  isLoggedIn = false;
  ngOnInit(): void {
    if(localStorage.getItem('uid') != null){
      this.isLoggedIn = true;
      console.log("User is : "+this.isLoggedIn);
    }
    else{
      this.isLoggedIn = false;
      console.log("User is : "+this.isLoggedIn);
    }
  }
}
