import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ReadInventoryComponent } from './read-inventory.component';

describe('ReadInventoryComponent', () => {
  let component: ReadInventoryComponent;
  let fixture: ComponentFixture<ReadInventoryComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ReadInventoryComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ReadInventoryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
