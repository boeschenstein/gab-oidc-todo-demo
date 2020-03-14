import { Component, OnInit } from '@angular/core';
import { ToDoItem } from '../todo-item';
import { TodoRepository } from '../todo-repository.service';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';

@Component({
  selector: 'app-todo-create',
  templateUrl: './todo-create.component.html',
  styleUrls: ['./todo-create.component.css']
})
export class TodoCreateComponent implements OnInit {

  toDoForm: FormGroup;

  constructor(private fb: FormBuilder, private repository: TodoRepository) {
    this.toDoForm = fb.group({
      text: ['', [Validators.required, Validators.minLength(10)]],
      completeUntil: new Date(),
      name: ['', [Validators.required, Validators.minLength(5)]],
      email: ['', [Validators.email]]
    });
   }

  submit() {
    if (!this.toDoForm.valid) {
      return;
    }

    const value = this.toDoForm.value;
    const toDoItem: ToDoItem = {
      id: 0,
      text: value.text,
      completeUntil: value.completeUntil,
      name: value.name,
      email: value.email
    };

    this.repository.add(toDoItem).subscribe();

    this.toDoForm.reset();
    this.toDoForm.patchValue({
      completeUntil: new Date()
    });
  }

  ngOnInit(): void {
  }

  get text() {
    return this.toDoForm.get('text');
  }

  get name() {
    return this.toDoForm.get('name');
  }

  get email() {
    return this.toDoForm.get('email');
  }

}
