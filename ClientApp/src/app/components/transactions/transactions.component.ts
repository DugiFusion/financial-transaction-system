import { Component, OnInit, AfterViewInit, ViewChild } from '@angular/core';
import { MatPaginator, MatPaginatorModule } from '@angular/material/paginator';
import { MatTableDataSource, MatTableModule } from '@angular/material/table';
import { TransactionsService } from '../../services/transactions/transactions.service';
import { Transaction } from '../../models/transaction';
import { DatePipe } from '@angular/common';

@Component({
  selector: 'app-transactions',
  standalone: true,
  imports: [
    DatePipe,
    MatPaginatorModule,
    MatTableModule
  ],
  templateUrl: './transactions.component.html',
  styleUrls: ['./transactions.component.css']
})

export class TransactionsComponent implements OnInit, AfterViewInit {
  displayedColumns: string[] = ['index', 'cardType', 'amount', 'customerId', 'type', 'createdDate', 'note'];
  dataSource = new MatTableDataSource<Transaction>([]);

  @ViewChild(MatPaginator) paginator!: MatPaginator;

  constructor(private readonly transactionsService: TransactionsService) { }

  ngOnInit(): void {
    const userId = '12cd25aa-4022-4a0c-a173-e597b390dcc3';
    this.transactionsService.get(userId).subscribe({
      next: (data) => {
        this.dataSource.data = data;
      },
      error: (err) => console.error('Error:', err),
    });
  }

  ngAfterViewInit(): void {
    this.dataSource.paginator = this.paginator;
  }
}
