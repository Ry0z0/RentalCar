import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MybookingDetailComponent } from './mybooking-detail.component';

describe('MybookingDetailComponent', () => {
  let component: MybookingDetailComponent;
  let fixture: ComponentFixture<MybookingDetailComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [MybookingDetailComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(MybookingDetailComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
