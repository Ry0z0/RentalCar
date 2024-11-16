import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { Router, NavigationEnd } from '@angular/router';
import { RouterModule } from '@angular/router';
import { HttpClientModule } from '@angular/common/http';
import { FormsModule } from '@angular/forms';
import { HeaderComponent } from './shared/components/header/header.component';
import { FooterComponent } from './shared/components/footer/footer.component';

@Component({
  selector: 'app-root',
    templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {
  standalonePages: string[] = ['/login-signup'];

  constructor(private router: Router) {}

  isStandalonePage(): boolean {
    return this.standalonePages.includes(this.router.url);
  }
}

