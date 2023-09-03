output "queue_arn" {
  value = module.standard.queue_arn
}

output "queue_dlq_arn" {
  value = module.standard.queue_dlq_arn
}

output "default_receive_policy_document" {
  value = module.standard.default_receive_policy_document
}

output "default_send_policy_document" {
  value = module.standard.default_send_policy_document
}

output "default_replay_policy_document" {
  value = module.standard.default_replay_policy_document
}