import { Component } from '@angular/core';
import { DatePipe } from "@angular/common";
import { RouterLink } from "@angular/router";
import { environment } from '../../../environments/environment';
@Component({
  selector: 'app-footer',
  imports: [DatePipe, RouterLink],
  templateUrl: './footer.component.html',
  styleUrl: './footer.component.css'
})
export class FooterComponent {
  today: number = Date.now();
  environmentName = environment.env;
}
