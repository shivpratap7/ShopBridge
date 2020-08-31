import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Inventory } from '../model/Inventory.model';
import { appSettings } from '../appSettings';
import { Observable, of } from 'rxjs';
import { map } from 'rxjs/operators';
import { Http, Response, Headers } from '@angular/http';
@Injectable({
    providedIn: 'root'
})
export class InventoryService {


    public controllername: string = "Inventory"

    constructor(private http: HttpClient,
        private appSettings: appSettings) { }
  


    public getInventory(Inventory:any): Observable<any> {
        if (Inventory){
            return this.http.get(this.appSettings.API_Config + this.controllername + '/GetInventory', Inventory);
        }
    }

    public createInventory(item: any) {
        if (item) {
            return this.http.post(this.appSettings.API_Config + this.controllername + '/CreateInventory', item);
        }
    }

    public updateInventory(Inventory: any) {

        if (Inventory) {
     
            return this.http.post(this.appSettings.API_Config + this.controllername + '/UpdateInventory', Inventory);

        }
    }

    public deleteInventory(Inventory: any) {

        if (Inventory) {

            return this.http.post(this.appSettings.API_Config + this.controllername + '/DeleteInventory', Inventory);

        }
    }

    public readInventory(Inventory: any) {

        if (Inventory) {

            return this.http.post(this.appSettings.API_Config + this.controllername + '/ReadInventory', Inventory);

        }
    }
   
  

}  