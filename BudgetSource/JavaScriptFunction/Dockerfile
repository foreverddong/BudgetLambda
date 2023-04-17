
FROM ghcr.io/openfaas/of-watchdog:0.9.11 as watchdog
FROM registry.donglinxu.com/budget/javascriptlambdamap:latest AS build

WORKDIR /app
COPY "handler.js" .

COPY --from=watchdog /fwatchdog /usr/bin/fwatchdog
RUN chmod +x /usr/bin/fwatchdog

RUN echo $PATH
ENV mode="http"
ENV upstream_url="http://127.0.0.1:4001"
ENV fprocess="node index.js"
CMD ["fwatchdog"]