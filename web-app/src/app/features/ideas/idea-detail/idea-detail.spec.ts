import { ComponentFixture, TestBed } from '@angular/core/testing';

import { IdeaDetail } from './idea-detail';

describe('IdeaDetail', () => {
  let component: IdeaDetail;
  let fixture: ComponentFixture<IdeaDetail>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [IdeaDetail],
    }).compileComponents();

    fixture = TestBed.createComponent(IdeaDetail);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
