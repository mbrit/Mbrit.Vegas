import { NgModule } from '@angular/core';
import { CommonModule, NgFor, NgIf } from '@angular/common';
import { FormsModule } from '@angular/forms';

@NgModule({
  declarations: [],
  imports: [
    CommonModule,
    FormsModule,
    NgFor,
    NgIf,
  ],
  exports: [
    CommonModule,
    FormsModule,
    NgFor,
    NgIf,
  ]
})
export class SharedModule { }
