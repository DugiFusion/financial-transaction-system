import { Component, OnInit, AfterViewInit, ViewChild } from '@angular/core';
import { MatPaginator, MatPaginatorModule } from '@angular/material/paginator';
import { MatTableDataSource, MatTableModule } from '@angular/material/table';
import { TransactionsService } from '../../services/transactions/transactions.service';
import { Transaction } from '../../models/transaction';
import { CommonModule, DatePipe, TitleCasePipe } from '@angular/common';
import { TransactionType } from '../../models/enums/transaction-type';
import { FormsModule } from '@angular/forms';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { ReportingService } from '../../services/reporting/reporting.service';


@Component({
  selector: 'app-transactions',
  standalone: true,
  imports: [
    DatePipe,
    MatPaginatorModule,
    MatTableModule,
    FormsModule,
    TitleCasePipe,
    CommonModule,
    MatIconModule,
    MatInputModule,
    MatSnackBarModule

  ],
  templateUrl: './transactions.component.html',
  styleUrls: ['./transactions.component.css']
})

export class TransactionsComponent implements OnInit, AfterViewInit {
  displayedColumns: string[] = ['index',  'amount', 'customerId', 'type', 'createdDate', 'note', 'actions']; // 'cardType',
  transactionToInsert: Transaction = {} as Transaction;
  dataSource = new MatTableDataSource<Transaction>([]);
  transactionTypes = Object.keys(TransactionType).filter(key => isNaN(Number(key)));
  allTransactions: Transaction[] = [];



  @ViewChild(MatPaginator) paginator!: MatPaginator;

  constructor(private readonly transactionsService: TransactionsService,
     private snackBar: MatSnackBar,
     private readonly reportingService: ReportingService
    ) { }

  ngOnInit(): void {
    const userId = '1234123412341234';
    this.transactionsService.get(userId).subscribe({
      next: (data) => {
        data.sort((a, b) => new Date(b.createdDate).getTime() - new Date(a.createdDate).getTime());
        data.forEach(transaction => {
          transaction.customerId = this.formatPAN(transaction.customerId);
        });

        this.dataSource.data = data;
        this.allTransactions = data;
      },
      error: (err) => console.error('Error:', err),
    });
  }

  ngAfterViewInit(): void {
    this.dataSource.paginator = this.paginator;
  }

  formatPAN(pan: string): string {
    if (pan.includes('-')) {
      return pan;
    }
    return pan.replace(/(.{4})/g, '$1-').slice(0, -1);
  }

  checkPAN(pan: string): boolean {
    return pan.match(/^\d{16}$/) !== null;
  }

  deleteTransaction(id: string): void {
    this.transactionsService.delete(id).subscribe({
      next: () => {
        console.log('Transaction deleted successfully');
        this.snackBar.open('Transaction deleted successfully', 'Close', {
          duration: 3000, // Duration in milliseconds
        });
      this.refreshTable();

      },
      error: (err) => console.error('Error deleting transaction:', err),
    });
  }

  insertTransaction(): void {
    this.transactionToInsert.note = this.transactionToInsert.note.trim();
    this.transactionToInsert.accountId = this.transactionToInsert.accountId.trim();
    this.transactionToInsert.customerId = this.transactionToInsert.customerId.trim();

    const cleanCustomerId = this.transactionToInsert.customerId.replace(/-/g, '');
    const cleanAccountId = this.transactionToInsert.accountId.replace(/-/g, '');

    if (!this.checkPAN(cleanCustomerId)) {
      console.error('Invalid PAN:', this.transactionToInsert.customerId);
      return;
    }
    if (!this.checkPAN(cleanAccountId)) {
      console.error('Invalid PAN:', this.transactionToInsert.customerId);
      return;
    }

    this.transactionToInsert.customerId = cleanCustomerId;
    this.transactionToInsert.accountId = cleanAccountId;

    
    this.transactionsService.insert(this.transactionToInsert).subscribe({
      next: (data) => {
        console.log('Transaction inserted successfully:', data);
        this.snackBar.open('Transaction inserted successfully', 'Close', {
          duration: 3000, // Duration in milliseconds
        });
        this.refreshTable(); // Refresh the table to show the new data
      },
      error: (err) => console.error('Error inserting transaction:', err),
    });
  }

  // Refresh table after insertion
  private refreshTable(): void {
    this.transactionsService.get('12cd25aa-4022-4a0c-a173-e597b390dcc3').subscribe({
      next: (data) => {
        data.forEach(transaction => {
          transaction.customerId = this.formatPAN(transaction.customerId);
        });
        this.dataSource.data = data;
      },
      error: (err) => console.error('Error refreshing table:', err),
    });
  }

  generateReport(): void{
    this.transactionsService.generateReport(this.allTransactions).subscribe({
      next: (data) => {
        console.log('Report generated successfully:', data);
        this.snackBar.open('Report generated successfully', 'Close', {
          duration: 3000, // Duration in milliseconds
        });
      },
      error: (err) => console.error('Error generating report:', err),
    });
  }

  
}
