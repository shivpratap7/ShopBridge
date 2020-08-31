import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { CreateInventoryComponent } from './create-inventory/create-inventory.component';
import { ListInventoryComponent } from './list-inventory/list-inventory.component';
import { ReadInventoryComponent } from './read-inventory/read-inventory.component';
import { CommonModule } from '@angular/common';
export const routes: Routes = [
    { path: '', component: CreateInventoryComponent, pathMatch: 'full' },
    { path: 'create-inventory', component: CreateInventoryComponent },
    { path: 'list-inventory/:id', component: ListInventoryComponent },
    { path: 'read-inventory/:id', component: ReadInventoryComponent }

];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
