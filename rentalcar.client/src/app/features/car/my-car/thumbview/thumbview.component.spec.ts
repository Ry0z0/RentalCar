import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ThumbviewComponent } from './thumbview.component';

describe('ThumbviewComponent', () => {
  let component: ThumbviewComponent;
  let fixture: ComponentFixture<ThumbviewComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ThumbviewComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ThumbviewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
