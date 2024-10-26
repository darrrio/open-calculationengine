namespace Open.CalculationEngine.Test;

public static class TestHelper{
    public const string EvaluateInputData = """
                                            {
                                                "$PRM1.VOLUMEBASE":6405.00,
                                                "$PRM1.DESCRIPTION":"SCUOLE ELEMENTARI",
                                                "$PRM1.PLANT":1,
                                                "$PRM1.CODE":"L4-023-115-01",
                                                   "SERVICES":[
                                                       {
                                                           "$PRM2.ID":1,
                                                           "$PRM2.CODE":"ENERGIA_SIE3",
                                                           "$PRM2.DESCRIPTION":"SERVIZIO ENERGIA EA",
                                                           "$PRM2.DATESTART":"2023-07-01",
                                                           "$PRM2.DATEEND":"2035-06-30",
                                                           "BILLINGUNITS":[
                                                               {
                                                                   "$PRM3.DATESTART":"2023-07-01",
                                                                   "$PRM3.DATEEND":"2035-06-30",
                                                                   "$PRM3.COMFORTTEMPERATURE":20.0,
                                                                   "$PRM3.COMFORTTEMPERATUREOPERATIVE":20.0,
                                                                   "$PRM3.JPKST":456470009.0,
                                                                   "$PRM3.DAYDEGREEOFFER":2016.0,
                                                                   "$PRM3.VOLUMESUMMERDELTA":0.0,
                                                                   "$PRM3.VOLUMESEASONDELTA":0.0,
                                                                   "$PRM3.HOURSOFFER":9.2,
                                                                   "$PRM3.ALFA":10.0,
                                                                   "$PRM3.TARIFF":0.7894
                                                               }
                                                           ]
                                                       }
                                                   ],
                                                   "$PRM1.THERMALCAPACITY":"HIGH"
                                            }
                                            """;

    public const string EvaluateComplexExpressionInputExpression = "$OP.MULTIPLY($OP.SUM($PRM3.JPKST;$PRM3.DAYDEGREEOFFER;);$FN.TARIFF($PRM1.PLANT;))";
    public const double EvaluateComplexExpressionResult = 360339016.53500;
    public const string EvaluateSimpleExpressionInputExpression = "$OP.SUM($PRM3.JPKST;$PRM3.DAYDEGREEOFFER;)";
    public const double EvaluateSimpleExpressionResult = 456472025.0;
    public const string EvaluateParameterExpressionInputExpression = "$PRM3.JPKST";
    public const double EvaluateParameterExpressionResult = 456470009.0;
}