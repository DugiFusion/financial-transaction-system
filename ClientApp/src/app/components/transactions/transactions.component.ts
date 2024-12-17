import { Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator, MatPaginatorModule } from '@angular/material/paginator';
import { MatTableDataSource, MatTableModule } from '@angular/material/table';
import { TransactionsService } from '../../services/transactions/transactions.service';
import { Transaction } from '../../models/transaction';
import { DatePipe, NgFor } from '@angular/common';

@Component({
  selector: 'app-transactions',
  standalone: true,
  imports: [
    NgFor,
    DatePipe,
    MatPaginatorModule,
    MatTableModule
  ],
  templateUrl: './transactions.component.html',
  styleUrls: ['./transactions.component.css']
})
export class TransactionsComponent implements OnInit {
  displayedColumns: string[] = ['index', 'cardType', 'amount', 'customerId', 'type', 'createdDate', 'note'];
  dataSource = new MatTableDataSource<Transaction>([]);

  @ViewChild(MatPaginator) paginator!: MatPaginator;

  constructor(private readonly transactionsService: TransactionsService) { }

  ngOnInit(): void {
    this.transactionsService.get().subscribe({
      next: (data) => {
        this.dataSource.data = data;
        this.dataSource.paginator = this.paginator;
      },
      error: (err) => console.error('Error:', err),
    });
  }
}
