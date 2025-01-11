# Step 1: Build the React app
FROM node:22 AS build

# Set the working directory in the container
WORKDIR /app
RUN npm config set legacy-peer-deps true
RUN npm install --save-dev ajv@^7
# Install dependencies
COPY package.json package-lock.json ./
RUN npm install

# Copy the rest of the app's source code
COPY . ./

# Build the app
RUN npm run build

# Step 2: Serve the React app using Nginx
FROM nginx:alpine

# Copy the build output from the previous stage to the Nginx HTML directory
COPY --from=build /app/build /usr/share/nginx/html

# Expose port 80 for the Nginx server
EXPOSE 80

# Start the Nginx server
CMD ["nginx", "-g", "daemon off;"]

