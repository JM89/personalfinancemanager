FROM node:24-alpine

WORKDIR /app
COPY package.json package-lock.json ./
RUN npm ci

COPY . ./
EXPOSE 3000
CMD ["npm", "run", "start", "--", "--host", "0.0.0.0", "--port", "3000", "--no-open", "--poll", "1000"]
