namespace EngineeringMath

open EngineeringMath.Common
open EngineeringMath
open EngineeringMath.Thermo



module SteamTable =
    let performPtvEntryQuery (q:PtvEntryQuery) =
        match q with
        | PtvEntryQuery.PtQuery (p, t) -> 
            SteamProperties.performSteamQuery (PtvQuery (performPtvEntryQuery (ValidatedPtvEntryQuery.BasicPtvEntryQuery (ValidatedBasicPtvEntryQuery.PtQuery (p, t)))))
        | PtvEntryQuery.SatTempQuery (t, r) -> 
            SteamProperties.performSteamQuery (PtvQuery (performPtvEntryQuery (ValidatedPtvEntryQuery.BasicPtvEntryQuery (ValidatedBasicPtvEntryQuery.SatTempQuery (t, r)))))
        | PtvEntryQuery.SatPreQuery (p, r) ->
            SteamProperties.performSteamQuery (PtvQuery (performPtvEntryQuery (ValidatedPtvEntryQuery.BasicPtvEntryQuery (ValidatedBasicPtvEntryQuery.SatPreQuery (p, r)))))
        | PtvEntryQuery.EnthalpyQuery (h, p) -> 
            SteamProperties.performSteamQuery (PtvQuery (performPtvEntryQuery (ValidatedPtvEntryQuery.EnthalpyQuery (h, p))))
        | PtvEntryQuery.EntropyQuery (s, p) ->
            SteamProperties.performSteamQuery (PtvQuery (performPtvEntryQuery (ValidatedPtvEntryQuery.EntropyQuery (s, p))))
        |> AsyncResult.map PtvEntry.mapFromValidatedEntry
        |> AsyncResult.mapError UiMessage.mapFromDomainError
