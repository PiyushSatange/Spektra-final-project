import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-admin-resources',
  templateUrl: './admin-resources.component.html',
  styleUrl: './admin-resources.component.css'
})
export class AdminResourcesComponent implements OnInit{

  activeBuckets: any;
  destroyedBuckets: any;
  constructor(private http: HttpClient){}

  ngOnInit(): void {
    this.http.get("https://localhost:7072/api/Bucket/GetAllActiveBuckets")
    .subscribe((data) => {
      this.activeBuckets = data;
      console.log(this.activeBuckets);
    })

    this.http.get("https://localhost:7072/api/Bucket/GetAllDestroyedBuckets")
    .subscribe((data) => {
      this.destroyedBuckets = data;
      console.log(this.destroyedBuckets);
    })
    
  }
}
