// type CustomRecord = Record<string, number | string>;
// export class Context {
//     private _baseData: Record<string, number | string | CustomRecord[]>;

//     constructor(baseData: object) {
//         this._baseData = JSON.parse(JSON.stringify(baseData));
//     }
//     get data() {
//         return JSON.parse(JSON.stringify(this._baseData));
//     }
//     getTariff(): number {
//         return JSON.parse(this.data).SERVICES[0].BILLINGUNITS[0]["$PRM3.TARIFF"] as number;
//     }
//     countSeasonDays(plantId: number): number {
//         const diff = Math.abs(new Date(Date.now()).getTime() - new Date(JSON.parse(this.data).SERVICES[0].BILLINGUNITS[0]["$PRM3.DATESTART"]).getTime());
//         const diffDays = Math.ceil(diff / (1000 * 3600 * 24));
//         return diffDays * plantId;
//     }
// }

using System.Collections;

public class Context
{
    public Hashtable data { get; set; }
    public Context(Hashtable baseData)
    {
        data = baseData;
    }
    public decimal GetTariff(){
        return Convert.ToDecimal(data["$PRM3.TARIFF"]);
    }

}