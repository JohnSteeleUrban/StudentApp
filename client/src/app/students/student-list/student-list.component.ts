import { Component, OnInit, ViewChild, AfterViewInit, ChangeDetectorRef } from '@angular/core';
import { RepositoryService } from 'src/app/services/repository.service';
import { MatSort } from '@angular/material/sort';
import { MatPaginator } from '@angular/material/paginator';
import { of as observableOf, merge, Observable } from 'rxjs';
import {catchError, map, startWith, switchMap} from 'rxjs/operators';
import { StudentApiResponse } from 'src/app/shared/interfaces';
import { MatDialog } from '@angular/material';
import { EditStudentComponent } from '../edit-student/edit-student.component';
import { MessagesService } from 'src/app/services/messages-service/messages.service';

@Component({
  selector: 'app-student-list',
  templateUrl: './student-list.component.html',
  styleUrls: ['./student-list.component.css']
})
export class StudentListComponent implements AfterViewInit {

  public displayedColumns = ['firstName', 'lastName', 'email', 'lastUpdated', 'update', 'delete'];
  public dataSource: StudentApiResponse[] = [];
  private editStudentComponent = EditStudentComponent;
  resultsLength = 0;
  isLoadingResults = true;

  @ViewChild(MatSort) sort: MatSort;
  @ViewChild(MatPaginator) paginator: MatPaginator;

  constructor(private repositoryService: RepositoryService, public dialog: MatDialog,private messagesService: MessagesService, private changeDetectorRefs: ChangeDetectorRef) {}

  ngAfterViewInit() {
    this.changeDetectorRefs.detectChanges();
    this.sort.sortChange.subscribe(() => this.paginator.pageIndex = 0);
    merge(this.sort.sortChange, this.paginator.page)
    .pipe(
      startWith({}),
      switchMap(() => {
        this.isLoadingResults = true;
        return this.getAllStudents(this.sort.active, this.sort.direction, this.paginator.pageIndex, this.paginator.pageSize);
      }),
      map(data => {
        // Flip flag to show that loading has finished.
        this.isLoadingResults = false;
        this.resultsLength = data.totalCount;

        return data.items;
      }),
      catchError(() => {
        this.isLoadingResults = false;
       // this.isRateLimitReached = true;
        return observableOf([]);
      })
    ).subscribe(data => this.dataSource = data);
 }

  public getAllStudents  (sort: string, order: string, page: number, pageSize: number): Observable<any>  {
    if(order === "desc"){ sort + " " + order;}
    return this.repositoryService.getData(`api/students?&sort=${sort}&page=${page + 1}&pageSize=${pageSize}`);
  }  

  public edit(recordId) {
    this.dialog.open(this.editStudentComponent, {
      data: {recordId: recordId,  paginator: this.paginator, dataSource: this.dataSource}
    });
  }
  
  public add() {
    this.dialog.open(this.editStudentComponent);
    this.getAllStudents(this.sort.active, this.sort.direction, this.paginator.pageIndex, this.paginator.pageSize);
    
  }  

  public delete(id: number) {
    this.repositoryService.delete(`api/students/${id}`);
    //this.ngAfterViewInit();
    this.messagesService.openDialog('Success!', 'Deleted from database.');
  }    
}