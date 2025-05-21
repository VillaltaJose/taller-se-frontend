import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ListadoTicketsComponent } from './listado-tickets.component';

describe('ListadoTicketsComponent', () => {
  let component: ListadoTicketsComponent;
  let fixture: ComponentFixture<ListadoTicketsComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [ListadoTicketsComponent]
    });
    fixture = TestBed.createComponent(ListadoTicketsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
