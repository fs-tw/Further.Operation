import { Component, OnInit } from '@angular/core';
import { OperationService } from '../services/operation.service';

@Component({
  selector: 'lib-operation',
  template: ` <p>operation works!</p> `,
  styles: [],
})
export class OperationComponent implements OnInit {
  constructor(private service: OperationService) {}

  ngOnInit(): void {
    this.service.sample().subscribe(console.log);
  }
}
