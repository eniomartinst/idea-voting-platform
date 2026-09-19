import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class IdeaService {
  // Aponta para o seu contêiner C# (cis-api) na porta 8005
  private apiUrl = 'http://localhost:8005/api';

  constructor(private http: HttpClient) {}

  // Ajuste as rotas '/ideas' ou '/IdeaQuery' conforme os controllers do seu C#
  getIdeas(): Observable<any[]> {
    return this.http.get<any[]>(`${this.apiUrl}/ideas`);
  }

  vote(ideaId: string): Observable<any> {
    return this.http.post(`${this.apiUrl}/ideas/${ideaId}/vote`, {});
  }
}