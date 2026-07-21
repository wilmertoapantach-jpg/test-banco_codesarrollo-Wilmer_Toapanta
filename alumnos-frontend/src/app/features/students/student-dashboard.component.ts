import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import {
  FormsModule,
  ReactiveFormsModule,
  FormBuilder,
  FormGroup,
  Validators,
} from '@angular/forms';
import { TableModule } from 'primeng/table';
import { ButtonModule } from 'primeng/button';
import { DialogModule } from 'primeng/dialog';
import { InputTextModule } from 'primeng/inputtext';
import { CheckboxModule } from 'primeng/checkbox';
import { ListboxModule } from 'primeng/listbox';
import { SelectModule } from 'primeng/select';
import { TextareaModule } from 'primeng/textarea';
import { MessageService } from 'primeng/api';
import { ToastModule } from 'primeng/toast';

import { StudentService } from '../../core/services/student.service';
import { Student } from '../../core/interface/student.interface';

@Component({
  selector: 'app-student-dashboard',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    TableModule,
    ButtonModule,
    DialogModule,
    InputTextModule,
    CheckboxModule,
    ListboxModule,
    SelectModule,
    TextareaModule,
    ToastModule,
  ],
  providers: [MessageService],
  templateUrl: './student-dashboard.component.html',
  styleUrls: ['./student-dashboard.component.css'],
})
export class StudentDashboardComponent implements OnInit {
  students: Student[] = [];
  availableStudents: Student[] = [];
  selectedAvailableStudent: Student | null = null;
  chosenStudents: Student[] = [];
  selectedChosenStudent: Student | null = null;

  showAddModal = false;
  studentForm: FormGroup;
  loading = false;
  totalRecords = 0;
  pageSize = 10;
  currentPage = 1;
  isAscending = true;

  constructor(
    private studentService: StudentService,
    private fb: FormBuilder,
    private messageService: MessageService,
    private cdr: ChangeDetectorRef,
  ) {
    this.studentForm = this.fb.group({
      name: ['', Validators.required],
      isActive: [true],
      description: ['', Validators.required],
    });
  }

  ngOnInit(): void {
    // Se ejecuta al inicializar el componente; la carga inicial se maneja desde la tabla.
  }

  // Recibe el evento de paginación de PrimeNG y calcula la página y tamaño actual.
  onLazyLoad(event: any) {
    this.currentPage = Math.floor((event.first ?? 0) / (event.rows ?? 10)) + 1;
    this.pageSize = event.rows ?? 10;
    this.loadStudents();
  }

  // Solicita los estudiantes al backend usando la página y tamaño actuales.
  loadStudents() {
    this.loading = true;
    this.studentService
      .listStudents({ pageNumber: this.currentPage, pageSize: this.pageSize })
      .subscribe({
        next: (res) => {
          queueMicrotask(() => {
            if (res.isSuccess && res.result) {
              this.students = [...res.result.items];
              this.totalRecords = res.result.count ?? 0;
              this.updateAvailableStudents();
            }
            this.loading = false;
            this.cdr.markForCheck();
          });
        },
        error: () => {
          queueMicrotask(() => {
            this.messageService.add({
              severity: 'error',
              summary: 'Error',
              detail: 'No se pudieron cargar los estudiantes',
            });
            this.loading = false;
            this.cdr.markForCheck();
          });
        },
      });
  }

  // Actualiza la lista de estudiantes disponibles excluyendo los ya seleccionados y solo dejando activos.
  updateAvailableStudents() {
    this.availableStudents = this.students.filter(
      (s) => s.isActive && !this.chosenStudents.some((cs) => cs.id === s.id),
    );
  }

  // Cambia el orden de la lista de nombres entre ascendente y descendente en cada clic.
  sortStudentsAlphabetically() {
    const direction = this.isAscending ? 1 : -1;
    this.students = [...this.students].sort((a, b) => a.name.localeCompare(b.name) * direction);
    this.isAscending = !this.isAscending;
    this.cdr.markForCheck();
  }

  // Mueve un estudiante desde la lista disponible a la lista de seleccionados al hacer doble clic.
  onAvailableStudentDoubleClick(event: MouseEvent, student: Student) {
    const index = this.availableStudents.findIndex((s) => s.id === student.id);
    if (index !== -1) {
      queueMicrotask(() => {
        this.chosenStudents = [...this.chosenStudents, student];
        this.updateAvailableStudents();
        this.cdr.markForCheck();
      });
    }
  }

  // Abre el modal de creación y reinicia el formulario con valores iniciales.
  openAddModal() {
    this.studentForm.reset({ isActive: true });
    this.showAddModal = true;
  }

  // Valida y envía el estudiante nuevo al backend para guardarlo.
  saveStudent() {
    if (this.studentForm.invalid) {
      this.studentForm.markAllAsTouched();
      return;
    }

    const newStudent = { ...this.studentForm.value, id: 0 };
    this.studentService.saveStudent(newStudent).subscribe({
      next: (res) => {
        if (res.isSuccess) {
          this.messageService.add({
            severity: 'success',
            summary: 'Éxito',
            detail: 'Estudiante guardado correctamente',
          });
          this.showAddModal = false;
          this.loadStudents(); // Reload grid
        } else {
          this.messageService.add({
            severity: 'error',
            summary: 'Error',
            detail: 'No se pudo guardar el estudiante',
          });
        }
      },
      error: () => {
        this.messageService.add({
          severity: 'error',
          summary: 'Error',
          detail: 'Error de servidor',
        });
      },
    });
  }
}
