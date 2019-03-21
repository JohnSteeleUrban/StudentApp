// export interface IStudent {
//     id: number;
//     lastName: string;
//     firstName: string;
//     email: string;
//     lastUpdated: any;
// }

// export class IPagination {
//     selectItemsPerPage: number[] = [5, 10, 25, 100];
//     pageSize = this.selectItemsPerPage[0];
//     pageIndex = 1;
//     allItemsLength = 0;
// }

export interface StudentApiResponse{
    items: Student[];
    totalCount: number;
  }
  
  export interface Student {
    id: number;
    lastName: string;
    firstName: string;
    email: string;
    lastUpdated: any;
    
  }