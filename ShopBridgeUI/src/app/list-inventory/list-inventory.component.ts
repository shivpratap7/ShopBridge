import { Component, OnInit, ViewChild } from '@angular/core';
import { ReactiveFormsModule, FormsModule, FormGroup, FormControl, Validators, FormBuilder, NgForm } from '@angular/forms';
import { InventoryService } from "../service/inventory.service";
import { Router, ActivatedRoute } from "@angular/router";
import { Observable, of } from 'rxjs';
import { Inventory } from '../model/Inventory.model';

declare var $;

@Component({
  selector: 'app-list-inventory',
  templateUrl: './list-inventory.component.html',
  styleUrls: ['./list-inventory.component.css']
})
  

export class ListInventoryComponent implements OnInit {
    public RtnData: any;
    public query: any;
    public Inventory: Inventory;
    public anyproduct: any

    inventory: Inventory[];
    editUserForm: boolean;
    userForm: boolean;
    editedUser: any = {};

    productForm: FormGroup;
    public products: any = [];
    public dataTable: any;
    public vm: { [key: string]: any } = {};
    isToggled: boolean = false;

    constructor(
        private fb: FormBuilder,
        private inventoryService: InventoryService,
        private router: Router
    ) {
        this.productForm = this.fb.group({
            id: ['', Validators.required],
            name: ['', Validators.required],
            description: ['', Validators.compose([Validators.required, Validators.minLength(10), Validators.maxLength(1000)])],
            price: ['', Validators.compose([Validators.required])],
        });
    }


    

    ngOnInit() {
        this.Init();
        this.loadProducts();
    }
    public Init(): void {

        this.query = {
            PageNumber: 1,
            PageSize: 9,
            SearchColumn: '',
            SearchText: '',
            SortColName: 'id',
            SortType: 'desc',
            TotalCount: 10
        };
        this.Inventory =
            {
                id: null,
                name: null,
                description: null,
                price: null
            }

    }
    public loadProducts(): Inventory[] {
        return Observable.bind(this.inventoryService.getInventory(this.query).subscribe(data => {
           //  this.RtnData = data.Data;
           //  this.query.TotalCount = data.DataCount;
            this.RtnData = data.Data;
            console.log(this.RtnData )
       }));
    }
    deleteProducts(id: any) {
        this.anyproduct = {
            anyproduct: id
        },

            console.log(this.anyproduct.id)
        this.inventoryService.deleteInventory(this.anyproduct)
            .subscribe(data => {
                this.ngOnInit();
                this.router.navigate(['list-inventory' + '/' + this.anyproduct]);

            });
    }
    public create() {
        this.router.navigate(['create-inventory']);


    }
    ViewProduct(id:any) {
        this.router.navigate(['read-inventory' + '/' + id]);
        
    }

    showEditUserForm(user: Inventory) {
        this.isToggled = !this.isToggled;
       if (!user) {
            this.userForm = false;
            return;
        }
        this.editUserForm = true;
        this.editedUser = user;
    }
    updateProduct(item: any) {
        this.anyproduct = item
       this.inventoryService.updateInventory(this.anyproduct).subscribe(result => {
           this.isToggled = false;
           this.router.navigate(['list-inventory' + '/' + this.anyproduct.id]);
          
        });
    }

}
