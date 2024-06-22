import { Component } from '@angular/core';
import { Router } from '@angular/router';
import  Swal  from 'sweetalert2'
@Component({
  selector: 'app-user-navbar',
  templateUrl: './user-navbar.component.html',
  styleUrl: './user-navbar.component.css'
})
export class UserNavbarComponent {
  isLogin = false;
  username : any;
  showUsername: boolean = true;

  toggleUsername(): void {
    this.showUsername = !this.showUsername; // Toggle the value to show/hide username
  }

  constructor(private router:Router){}
  ngOnInit(): void {
    if(localStorage.getItem('uid') != null){
      this.isLogin = true;
      console.log("User is Logged in: "+this.isLogin);
      this.username = localStorage.getItem("name");
    }
    else{
      this.isLogin = false;
      console.log("User is Logged in: "+this.isLogin);
    }
  }

  onLogout(){
    Swal.fire({
      title: "Are you sure?",
      text: "You will be logged out of you account!",
      icon: "warning",
      showCancelButton: true,
      confirmButtonColor: "#3085d6",
      cancelButtonColor: "#d33",
      confirmButtonText: "Yes, log out!"
    }).then((result) => {
      if (result.isConfirmed) {
        localStorage.removeItem("uid");
        localStorage.removeItem("name");
        this.router.navigate(['/login']);
        Swal.fire({
          title: "Logged Out!",
          text: "You are successfully signed out",
          icon: "success"
        });
      }
    });
  }
}
