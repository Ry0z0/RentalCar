import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ViewCardetailComponent } from './view-cardetail.component';

describe('ViewCardetailComponent', () => {
  let component: ViewCardetailComponent;
  let fixture: ComponentFixture<ViewCardetailComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ViewCardetailComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ViewCardetailComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
