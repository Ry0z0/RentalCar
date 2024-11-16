import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ListMybookingComponent } from './list-mybooking.component';

describe('ListMybookingComponent', () => {
  let component: ListMybookingComponent;
  let fixture: ComponentFixture<ListMybookingComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ListMybookingComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ListMybookingComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
