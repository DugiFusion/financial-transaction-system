import { Component, OnInit, AfterViewInit, ViewChild } from '@angular/core';
import { MatPaginator, MatPaginatorModule } from '@angular/material/paginator';
import { MatTableDataSource, MatTableModule } from '@angular/material/table';
import { TransactionsService } from '../../services/transactions/transactions.service';
import { Transaction } from '../../models/transaction';
import { CommonModule, DatePipe, TitleCasePipe } from '@angular/common';
import { TransactionType } from '../../models/enums/transaction-type';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-transactions',
  standalone: true,
  imports: [
    DatePipe,
    MatPaginatorModule,
    MatTableModule,
    FormsModule,
    TitleCasePipe,
    CommonModule

  ],
  templateUrl: './transactions.component.html',
  styleUrls: ['./transactions.component.css']
})

export class TransactionsComponent implements OnInit, AfterViewInit {
  displayedColumns: string[] = ['index', 'cardType', 'amount', 'customerId', 'type', 'createdDate', 'note'];
  transactionToInsert: Transaction = {} as Transaction;
  dataSource = new MatTableDataSource<Transaction>([]);
  transactionTypes = Object.values(TransactionType);




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

  insertTransaction(): void {
    this.transactionsService.insert(this.transactionToInsert).subscribe({
      next: (data) => {
        console.log('Transaction inserted successfully:', data);
        this.refreshTable(); // Refresh the table to show the new data
      },
      error: (err) => console.error('Error inserting transaction:', err),
    });
  }

  // Refresh table after insertion
  private refreshTable(): void {
    this.transactionsService.get('12cd25aa-4022-4a0c-a173-e597b390dcc3').subscribe({
      next: (data) => {
        this.dataSource.data = data;
      },
      error: (err) => console.error('Error refreshing table:', err),
    });
  }

  
}
