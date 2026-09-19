import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { IdeaService } from '../../../core/services/idea.service';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './dashboard.html',
  styleUrl: './dashboard.scss'
})
export class Dashboard implements OnInit {
  ideas: any[] = [];

  constructor(private ideaService: IdeaService) {}

  ngOnInit(): void {
    this.loadIdeas();
  }

  loadIdeas() {
    this.ideaService.getIdeas().subscribe({
      next: (data) => this.ideas = data,
      error: (err) => console.error('Erro ao carregar ideias', err)
    });
  }

  vote(ideaId: string) {
    this.ideaService.vote(ideaId).subscribe({
      next: () => this.loadIdeas(), // Atualiza a lista após votar
      error: (err) => console.error('Erro ao registrar voto', err)
    });
  }
}