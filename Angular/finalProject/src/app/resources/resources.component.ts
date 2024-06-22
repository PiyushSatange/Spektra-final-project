import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import Swal from 'sweetalert2';
@Component({
  selector: 'app-resources',
  templateUrl: './resources.component.html',
  styleUrl: './resources.component.css'
})
export class ResourcesComponent implements OnInit{
  isLoggedIn = false;
  buckets : any;
  azureCount: number = 0;
  awsCount: number = 0;
  gcpCount: number = 0;
  constructor(private http: HttpClient){}
  ngOnInit(): void {
    if(localStorage.getItem('uid') != null){
      this.isLoggedIn = true;
      console.log("User is : "+this.isLoggedIn);
      const email = localStorage.getItem("email");
      Swal.fire({
        title: 'Loading...',
        text: 'Please wait',
        allowOutsideClick: false,
        allowEscapeKey: false,
        showConfirmButton: false,
        timer: 1000,
        didOpen: () => {
          Swal.showLoading();
        }
      });
      this.http.get(`https://localhost:7072/api/Bucket/getByEmail?email=${email}`)
      .subscribe((data) => {
        console.log(data);
        this.buckets = data;
        for (let i = 0; i < this.buckets.length; i++) {
          if(this.buckets[i].platform == "AWS"){
            console.log("in aws");
            this.awsCount++;
          }
          else if(this.buckets[i].platform == "Azure"){
            console.log("in azure");
            this.azureCount++;
          }
          else if(this.buckets[i].platform == "GCP"){
            this.gcpCount++;
          }
        }
      })
    }
    else{
      this.isLoggedIn = false;
      console.log("User is : "+this.isLoggedIn);
    }
    
    
  }
}
