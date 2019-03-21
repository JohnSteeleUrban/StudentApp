import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { HttpClientModule } from '@angular/common/http';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { MaterialModule } from './material/material.module';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { StudentListComponent } from './students/student-list/student-list.component';
import { EditFormComponent } from './students/edit-form/edit-form.component';
import { EditStudentComponent } from './students/edit-student/edit-student.component';
import { MessagesComponent } from './services/messages-service/messages.component';
import { AddStudentComponent } from './students/add-student/add-student.component';

@NgModule({
  declarations: [
    AppComponent,
    StudentListComponent,
    EditFormComponent,
    EditStudentComponent,
    MessagesComponent,
    AddStudentComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    MaterialModule,
    BrowserAnimationsModule,
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
  ],
  providers: [],
  bootstrap: [AppComponent],
  entryComponents: [EditStudentComponent, MessagesComponent]
})
export class AppModule { }
