import { Component, OnInit, ViewChild } from '@angular/core';
import { Report } from '../../models/report';
import { ReportingService } from '../../services/reporting/reporting.service';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatTableDataSource, MatTableModule } from '@angular/material/table';
import { MatPaginator, MatPaginatorModule } from '@angular/material/paginator';
import { CommonModule, DatePipe, TitleCasePipe } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';

@Component({
  selector: 'app-reports',
  imports: [
    DatePipe,
    MatPaginatorModule,
    MatTableModule,
    FormsModule,
    CommonModule,
    MatIconModule,
    MatInputModule,
    MatSnackBarModule
],
  templateUrl: './reports.component.html',
  styleUrls: ['./reports.component.css']
})
export class ReportsComponent implements OnInit {
  displayedColumns: string[] = ['generatedAt', 'fileName', 'actions'];
  dataSource = new MatTableDataSource<Report>([]);

  @ViewChild(MatPaginator) paginator!: MatPaginator;

  constructor(private readonly reportingService: ReportingService,
         private snackBar: MatSnackBar,
  ) {}

  ngOnInit(): void {
    const userId = '1234123412341234';
    this.reportingService.get(userId).subscribe({
      next: (data) => {
        data.sort((a, b) => new Date(b.generatedAt).getTime() - new Date(a.generatedAt).getTime());
        this.dataSource.data = data;
      },
      error: (err) => console.error('Error:', err),
    });
  }

  ngAfterViewInit(): void {
    this.dataSource.paginator = this.paginator;
  }

  downloadReport(reportId: string): void {
    this.reportingService.getFile(reportId).subscribe({
      next: (data: any) => {
        console.log(data);
  
        // Check if the base64 string is valid
        if (!data.fileContents || !this.isBase64(data.fileContents)) {
          console.error('Invalid base64 string');
          return;
        }
  
        const byteCharacters = atob(data.fileContents); // Decode base64 string
        const byteNumbers = new Array(byteCharacters.length);
        for (let i = 0; i < byteCharacters.length; i++) {
          byteNumbers[i] = byteCharacters.charCodeAt(i);
        }
        const byteArray = new Uint8Array(byteNumbers);
        const blob = new Blob([byteArray], { type: 'text/csv' }); // Set MIME type to 'text/csv'
        const url = window.URL.createObjectURL(blob);
        const a = document.createElement('a');
        a.href = url;
        a.download = data.fileDownloadName; // Use the file name from the response
        document.body.appendChild(a);
        a.click();
        document.body.removeChild(a);
        window.URL.revokeObjectURL(url);
      },
      error: (err) => console.error('Error:', err),
    });
  }
  
  // Helper function to check if a string is valid base64
  isBase64(str: string): boolean {
    try {
      return btoa(atob(str)) === str;
    } catch (err) {
      return false;
    }
  }

  deleteReport(reportId: string): void {
    this.reportingService.delete(reportId).subscribe({
      next: () => {
        this.dataSource.data = this.dataSource.data.filter((report) => report.id !== reportId);
        this.snackBar.open('Reportr deleted successfully', 'Close', {
          duration: 3000, // Duration in milliseconds
        });
      this.refreshTable();
      },
      error: (err) => console.error('Error:', err),
    });
  }

  refreshTable(): void {
    const userId = '1234123412341234';
    this.reportingService.get(userId).subscribe({
      next: (data) => {
        data.sort((a, b) => new Date(b.generatedAt).getTime() - new Date(a.generatedAt).getTime());
        this.dataSource.data = data;
      },
      error: (err) => console.error('Error:', err),
    });
  }
}


