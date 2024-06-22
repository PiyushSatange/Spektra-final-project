import { Component } from '@angular/core';
import { Router } from '@angular/router';
import Swal from 'sweetalert2'; 

@Component({
  selector: 'app-admin-navbar',
  templateUrl: './admin-navbar.component.html',
  styleUrl: './admin-navbar.component.css'
})
export class AdminNavbarComponent {

  constructor(private router:Router){}
  logout(){
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
