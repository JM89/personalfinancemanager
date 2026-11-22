FROM node:26-alpine

RUN npm install --global npm@12.2.3

WORKDIR /app
COPY package.json package-lock.json ./
RUN npm ci

COPY . ./
EXPOSE 3000
CMD ["npm", "run", "start", "--", "--host", "0.0.0.0", "--port", "3000", "--no-open", "--poll", "1000"]
