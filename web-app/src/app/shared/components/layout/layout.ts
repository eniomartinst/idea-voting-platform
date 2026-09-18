import { Component } from '@angular/core';
import { RouterModule } from '@angular/router'; // Importação essencial para as rotas funcionarem

@Component({
  selector: 'app-layout',
  standalone: true,
  imports: [RouterModule], // Injetando as diretivas de router-outlet e routerLink no HTML
  templateUrl: './layout.html',
  styleUrl: './layout.scss'
})
export class Layout {

}