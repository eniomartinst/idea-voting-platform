import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, RouterModule } from '@angular/router';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { IdeaService } from '../../../core/services/idea';
import { IdeaResponseDto } from '../../../core/models/api.models';

@Component({
  selector: 'app-idea-detail',
  standalone: true,
  imports: [CommonModule, RouterModule, ReactiveFormsModule],
  templateUrl: './idea-detail.html',
  styleUrl: './idea-detail.scss'
})
export class IdeaDetail implements OnInit {
  topicId: string = '';
  ideas: IdeaResponseDto[] = [];
  ideaForm: FormGroup;

  constructor(
    private route: ActivatedRoute,
    private ideaService: IdeaService,
    private fb: FormBuilder
  ) {
    // Formulário simples com apenas o campo de conteúdo da ideia
    this.ideaForm = this.fb.group({
      content: ['', [Validators.required, Validators.minLength(5)]]
    });
  }

  ngOnInit(): void {
    // Captura o ID do tópico direto da URL (/topico/123)
    this.topicId = this.route.snapshot.paramMap.get('id') || '';
    if (this.topicId) {
      this.loadIdeas();
    }
  }

  loadIdeas(): void {
    this.ideaService.getIdeasByTopic(this.topicId).subscribe({
      next: (data: IdeaResponseDto[]) => this.ideas = data,
      error: (err: any) => console.error('Erro ao carregar ideias', err)
    });
  }

  onSubmitIdea(): void {
    if (this.ideaForm.valid) {
      this.ideaService.createIdea(this.topicId, this.ideaForm.value).subscribe({
        next: () => {
          this.ideaForm.reset(); // Limpa o formulário
          this.loadIdeas(); // Recarrega a lista para mostrar a nova ideia
        },
        error: (err: any) => console.error('Erro ao criar ideia', err)
      });
    }
  }

  vote(ideaId: string): void {
    this.ideaService.vote(ideaId).subscribe({
      next: () => this.loadIdeas(), // Atualiza a contagem de votos
      error: (err: any) => console.error('Erro ao registrar voto', err)
    });
  }
}