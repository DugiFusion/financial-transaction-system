import { Component, OnInit, ViewChild } from '@angular/core';
import { Report } from '../../models/report';
import { ReportingService } from '../../services/reporting/reporting.service';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatTableDataSource, MatTableModule } from '@angular/material/table';
import { MatPaginator, MatPaginatorModule } from '@angular/material/paginator';
import { CommonModule, DatePipe, TitleCasePipe } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MatSnackBarModule } from '@angular/material/snack-bar';

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
  displayedColumns: string[] = ['generatedAt', 'fileName', 'download'];
  dataSource = new MatTableDataSource<Report>([]);



  @ViewChild(MatPaginator) paginator!: MatPaginator;


  constructor(    private readonly reportingService: ReportingService
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

  downloadReport(reportId: number): void {
    // Implement download logic here
  }
}

// const REPORT_DATA: Report[] = [
//   { id: "1", generatedAt: new Date(), name: 'Report 1', accountId: '1234' },
//   { id: "2", generatedAt: new Date(), name: 'Report 2', accountId: '1234' },
//   // Add more report data here
// ];


