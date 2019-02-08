import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-Value',
  templateUrl: './Value.component.html',
  styleUrls: ['./Value.component.css']
})
export class ValueComponent implements OnInit {
values:any;

  constructor(private http:HttpClient) { }

  ngOnInit() {
    this.getValues();
  }
getValues(){
  this.http.get('http://localhost:5000/api/values').subscribe(x=>{
    this.values=x;
  }, 
  error=>{
    console.log(error);
  });
}
}
