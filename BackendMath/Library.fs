namespace EngineeringMath

open EngineeringMath.Common
open EngineeringMath
open EngineeringMath.Thermo



module SteamTable =
    let PerformPtvEntryQuery q : PerformPtvEntryQuery =
        match q with
        | PtQuery (p, t) -> 
            SteamProperties.performSteamQuery (PtvQuery (performPtvEntryQuery (ValidatedPtvEntryQuery.BasicPtvEntryQuery (ValidatedBasicPtvEntryQuery.PtQuery (p, t)))))
        | SatTempQuery (t, r) -> 
            SteamProperties.performSteamQuery (PtvQuery (performPtvEntryQuery (ValidatedPtvEntryQuery.BasicPtvEntryQuery (ValidatedBasicPtvEntryQuery.SatTempQuery (t, r)))))
        | SatPreQuery (p, r) ->
            SteamProperties.performSteamQuery (PtvQuery (performPtvEntryQuery (ValidatedPtvEntryQuery.BasicPtvEntryQuery (ValidatedBasicPtvEntryQuery.SatPreQuery (p, r)))))
        | EnthalpyQuery (h, p) -> 
            SteamProperties.performSteamQuery (PtvQuery (performPtvEntryQuery (ValidatedPtvEntryQuery.EnthalpyQuery (h, p))))
        | EntropyQuery (s, p) ->
            SteamProperties.performSteamQuery (PtvQuery (performPtvEntryQuery (ValidatedPtvEntryQuery.EntropyQuery (s, p))))
            
        