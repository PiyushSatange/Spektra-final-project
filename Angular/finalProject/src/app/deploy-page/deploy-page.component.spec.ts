import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DeployPageComponent } from './deploy-page.component';

describe('DeployPageComponent', () => {
  let component: DeployPageComponent;
  let fixture: ComponentFixture<DeployPageComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [DeployPageComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(DeployPageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
