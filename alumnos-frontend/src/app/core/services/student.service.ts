import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { Student, BaseResponse, PaginatedResult, StudentListRequest } from '../interface/student.interface';

@Injectable({
  providedIn: 'root'
})
export class StudentService {
  private apiUrl = environment.apiUrl + 'Student';

  constructor(private http: HttpClient) {}

  listStudents(request: StudentListRequest): Observable<BaseResponse<PaginatedResult<Student>>> {
    return this.http.post<BaseResponse<PaginatedResult<Student>>>(`${this.apiUrl}/ListStudents`, request);
  }

  saveStudent(student: Partial<Student>): Observable<BaseResponse<Student>> {
    return this.http.post<BaseResponse<Student>>(`${this.apiUrl}/SaveStudent`, student);
  }
}
