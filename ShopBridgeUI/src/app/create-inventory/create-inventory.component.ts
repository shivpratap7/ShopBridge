import { Component, OnInit } from '@angular/core';
import { ReactiveFormsModule, FormsModule, FormGroup, FormControl, Validators, FormBuilder, NgForm } from '@angular/forms';
import {InventoryService} from "../service/inventory.service";
import {Router} from "@angular/router";

@Component({
  selector: 'app-create-inventory',
  templateUrl: './create-inventory.component.html',
  styleUrls: ['./create-inventory.component.css']
})
export class CreateInventoryComponent implements OnInit {

    
    productForm: FormGroup;
    public vm: { [key: string]: any } = {};
    constructor(
        private fb: FormBuilder,
        private inventoryService: InventoryService,
        private router: Router) {
        this.productForm = this.fb.group({
            name: ['', Validators.required],
            description: ['', Validators.compose([Validators.required, Validators.minLength(10), Validators.maxLength(1000)])],
            price: ['', Validators.compose([Validators.required])],
        });
    }

    ngOnInit() {
    }

    saveProduct(values) {
        console.log(this.vm);
        this.inventoryService.createInventory(this.vm).subscribe(result => {
            this.router.navigate(['list-inventory/:id']);
        });
    }
}