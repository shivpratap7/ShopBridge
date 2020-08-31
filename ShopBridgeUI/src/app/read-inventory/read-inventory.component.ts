import { Component, OnInit } from '@angular/core';
import { InventoryService } from "../service/inventory.service";
import {Router, ActivatedRoute} from "@angular/router";
import { Inventory } from '../model/Inventory.model';
import { Observable, of } from 'rxjs';

@Component({
  selector: 'app-read-inventory',
  templateUrl: './read-inventory.component.html',
  styleUrls: ['./read-inventory.component.css']
})
export class ReadInventoryComponent implements OnInit {
    public RtnData: any;
    productID: any;
    productData: any;
    public query: any;
    public Inventory: Inventory;
    inventory: Inventory[];
    public name = localStorage.getItem("name");
    public description = localStorage.getItem("description");
    public price = localStorage.getItem("price");
    public Readproduct: any
    constructor(
        private inventoryService: InventoryService,
        private router: Router,
        private actRoute: ActivatedRoute) { }

    ngOnInit() {
        this.productID = this.actRoute.snapshot.params['id'];
        console.log(this.productID)
       // this.Init();
        this.loadProductDetails(this.productID);
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
    public loadProductDetails(product: any) {
        this.Readproduct = {
            Readproduct: product
        }

        return this.inventoryService.readInventory(this.Readproduct).subscribe(data => {
            //  this.RtnData = data.Data;
            //  this.query.TotalCount = data.DataCount;
            this.RtnData = data;
            localStorage.setItem('RtnData', this.RtnData.description);
            console.log(this.RtnData)
        });
    }
    navigation(link) {
        this.router.navigate([link]);
    }
}