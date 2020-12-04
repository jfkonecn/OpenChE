namespace EngineeringMath

type PureRegion = SupercriticalFluid|Gas|Vapor|Liquid|Solid
type PhaseRegion = PureRegion of PureRegion|SolidLiquid|LiquidVapor|SolidVapor|SolidLiquidVapor
