FROM mcr.microsoft.com/dotnet/core/sdk AS builder

ENV TARGET_OS=linux-x64 
#alpine.3.6-x64

WORKDIR /app

COPY . .

RUN dotnet publish -c Release -o build --self-contained -r $TARGET_OS ./src/HermesBroker/HermesBroker.csproj

####### SERVING STAGE #######

FROM ubuntu:latest

WORKDIR /broker

COPY --from=builder /app/build/* /broker/

# Seems that self-contained application .NET does not start in Alpine Linux. Ubuntu is necessary.
# If you want to try to use Alpine, add libstdc++
#RUN apk add libstdc++ 

EXPOSE 1883

CMD [ "./HermesBroker" ]
