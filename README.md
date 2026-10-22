# iels-fir-manager-api
This is the micro-service responsible for managing FIRs and FIR to Case transition .


# Generate migartion script 

dotnet ef migrations script \
--idempotent \
--project src/IELS.FIR.Infrastructure \
--startup-project src/IELS.FIR.Api \
--context FirDbContext \
--output fir-migration.sql